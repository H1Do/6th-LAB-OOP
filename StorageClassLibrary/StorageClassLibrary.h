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

/*public ref class Node {
public:
    CShape^ data;
    Node^ next;
};

public ref class Storage {
private:
    Node^ head;
    Node^ current;

public:
    Storage() {
        head = nullptr;
        current = nullptr;
    }

    bool isEmpty() {
        return head == nullptr;
    }

    void add(CShape^ shape) {
        Node^ node = gcnew Node();
        node->data = shape;
        node->next = nullptr;

        if (isEmpty()) {
            head = node;
        }
        else {
            Node^ current = head;

            while (current->next != nullptr) {
                current = current->next;
            }

            current->next = node;
        }
    }

    void clear() {
        while (!isEmpty()) {
            removeCurrent();
        }
    }

    void first() {
        current = head;
    }

    bool isLast() {
        if (isEmpty() || current == nullptr) {
            return false;
        }

        return current->next == nullptr;
    }

    void next() {
        if (current != nullptr) {
            current = current->next;
        }
    }

    CShape^ getCurrent() {
        if (current != nullptr) {
            return current->data;
        }
        return nullptr;
    }

    void removeCurrent() {
        if (isEmpty() || current == nullptr) {
            return;
        }

        if (current == head) {
            head = head->next;
            delete current;
            current = head;
            return;
        }

        Node^ prev = head;
        while (prev != nullptr && prev->next != current) {
            prev = prev->next;
        }

        if (prev == nullptr) {
            return;
        }

        prev->next = current->next;
        delete current;
        current = prev->next;
    }
};*/

public ref class IList
	{
	public:
		virtual void add(CShape^ obj) = 0;
		virtual void del(CShape^ obj) = 0;
		virtual CShape^ getObject() = 0;
		virtual void first() = 0;
		virtual void next() = 0;
		virtual bool isEOL() = 0;
	};

	public ref class MyStorage : public IList {

	protected:

		array<CShape^>^ data;
		int curr, size, count;

		void resize() {

			size++;

			array<CShape^>^ tmp = gcnew array<CShape^>(size);

			for (int i = 0; i < size - 1; i++)
				tmp[i] = data[i];

			data = tmp;
		}

	public:

		MyStorage() {
			curr = 0; size = 0; count = 0;
			data = gcnew array<CShape^>(size);
		}

		void add(CShape^ obj) override {

			if (this->isEOL())
			{
				if (count < curr) {
					for (int i = 0; i < size; i++)
						if (data[i] == nullptr) {
							data[i] = obj;
							count++;
							return;
						}
				}

				this->resize();
			}

			while (data[curr] != nullptr) {
				curr++;
				if (this->isEOL())
					this->resize();
			}

			data[curr] = obj;
			curr++;
			count++;
		}

		void del(CShape^ obj) override {
			delete obj;
			data[curr] = nullptr;
			count--;
		}

		CShape^ getObject() override {
			return data[curr];
		}

		void first() override {
			curr = 0;
		}

		void next() override {
			curr++;
		}

		bool isEOL() override {
			return curr > size - 1;
		}
	};
}