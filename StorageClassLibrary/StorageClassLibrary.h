#pragma once
using namespace System;

namespace StorageClassLibrary {
	public ref class CShape {
	protected:
		bool is_selected;
		String^ color;

	public:
		~CShape() { }
		virtual void Select() { is_selected = true; }
		virtual void Unselect() { is_selected = false; }
		virtual bool IsSelected() { return is_selected; }
		virtual void ChangeColor(String^ color) { this->color = color; }

		virtual void ChangeSize(char type) = 0;
		virtual bool WasClicked(int x, int y) = 0;
		virtual void Draw() = 0;
		virtual void Move(char direction) = 0;
		virtual bool CanMove(char direction) = 0;
	};

    public ref class Node
    {
    public:
        CShape^ shape;
        Node^ next;

        Node(CShape^ shape)
        {
            this->shape = shape;
            this->next = nullptr;
        }
    };

    public ref class List
    {
    private:
        Node^ head;
        Node^ tail;
        int count;

    public:
        List()
        {
            this->head = nullptr;
            this->tail = nullptr;
            this->count = 0;
        }

        void Add(CShape^ shape)
        {
            Node^ newNode = gcnew Node(shape);

            if (this->count == 0)
            {
                this->head = newNode;
                this->tail = newNode;
            }
            else
            {
                this->tail->next = newNode;
                this->tail = newNode;
            }

            this->count++;
        }

        void Remove(CShape^ shape)
        {
            Node^ current = this->head;
            Node^ previous = nullptr;

            while (current != nullptr)
            {
                if (current->shape == shape)
                {
                    if (previous != nullptr)
                    {
                        previous->next = current->next;

                        if (current == this->tail)
                        {
                            this->tail = previous;
                        }
                    }
                    else
                    {
                        this->head = current->next;

                        if (current == this->tail)
                        {
                            this->tail = nullptr;
                        }
                    }

                    this->count--;
                    break;
                }

                previous = current;
                current = current->next;
            }
        }

        void RemoveAt(int index)
        {
            if (index < 0 || index >= this->count)
            {
                throw gcnew IndexOutOfRangeException();
            }

            Node^ current = this->head;
            Node^ previous = nullptr;

            for (int i = 0; i < index; i++)
            {
                previous = current;
                current = current->next;
            }

            if (previous != nullptr)
            {
                previous->next = current->next;

                if (current == this->tail)
                {
                    this->tail = previous;
                }
            }
            else
            {
                this->head = current->next;

                if (current == this->tail)
                {
                    this->tail = nullptr;
                }
            }

            this->count--;
        }

        void Clear()
        {
            this->head = nullptr;
            this->tail = nullptr;
            this->count = 0;
        }

        int GetSize()
        {
            return this->count;
        }

        CShape^ Get(int index)
        {
            if (index < 0 || index >= this->count)
            {
                throw gcnew IndexOutOfRangeException();
            }

            Node^ current = this->head;
            int currentIndex = 0;

            while (current != nullptr)
            {
                if (currentIndex == index)
                {
                    return current->shape;
                }

                currentIndex++;
                current = current->next;
            }

            return nullptr;
        }
    };
}
