#pragma once

using namespace System;

namespace StorageClassLibrary {
	public ref class CShape
	{
	protected: 
		int x, y;
		bool is_selected;
		String^ color;

	public:
		~CShape() { }
		virtual void Select() { is_selected = true; }
		virtual void Unselect() { is_selected = false; }
		virtual bool IsSelected() { return is_selected; }
		virtual void ChangeColor(String^ color) { this->color = color; }

		virtual void ChangeSize(String^ type) = 0;
		virtual bool WasClicked(int x, int y) = 0;
		virtual void Draw() = 0;
		virtual void Move(char direction) = 0;
		virtual bool CanMove(char direction) = 0;
	};
	
	public ref class Storage {
	protected:
		ref class Node {
		public:
			Node^ next;
			CShape^ value;

			Node(CShape^ value) : value(value), next(nullptr) { }
			~Node() { }
			CShape^ getValue() { return value; }
		};

		Node^ first, ^ last, ^ current;
		size_t size, position;

	public:
		// Конструктор
		Storage() : first(nullptr), last(nullptr), size(0) { }
		~Storage() {
			while (first && size) {
				removeFront();
			}
		}

		bool isEmpty() {
			return first == nullptr;
		}

		// Получаем размер списка
		size_t getSize() {
			return size;
		}

		void setFirst() {
			position = 0;
		}

		bool isLast() {
			return current == last;
		}

		void next() {
			current = current->next;
			position += 1;
		}

		CShape^ getCurrent() {
			return current->getValue();
		}

		void deleteCurrent() {
			popNth(position);
		}

		// Вставка в конец
		void pushBack(CShape^ value) {
			Node^ newNode = gcnew Node(value);
			if (isEmpty()) {
				first = last = newNode;
			}
			else {
				last->next = newNode;
				last = newNode;
			}
			size++;
		}

		// Вставка в начало
		void pushFront(CShape^ value) {
			Node^ newNode = gcnew Node(value);
			if (isEmpty()) {
				first = last = newNode;
			}
			else {
				newNode->next = first;
				first = newNode;
			}
			size++;
		}

		// Вставка по n-ому индексу
		void pushNth(size_t position, CShape^ value) {
			if (position == 0) {
				pushFront(value);
				return;
			}
			else if (position == size - 1) {
				pushBack(value);
				return;
			}
			else if (position > size - 1) {
				return;
			}

			Node^ newNode = gcnew Node(value);
			Node^ temp = first;

			for (size_t i = 1; i < position; i++) {
				temp = temp->next;
			}

			newNode->next = temp->next;
			temp->next = newNode;
			size++;
		}

		// Возврат первого элемента
		CShape^ getFront() {
			return first->value;
		}

		// Возврат последнего элемента
		CShape^ getBack() {
			return last->value;
		}

		// Возврат n-ного элемента
		CShape^ getNth(size_t position) {
			Node^ result = first;
			for (int i = 0; i < position; i++) {
				result = result->next;
			}
			return result->value;
		}

		// Просто удаление без возврата первого элемента
		void removeFront() {
			if (!first) return;
			Node^ temp = first->next;
			delete first;
			first = temp;
			size--;
		}

		// Возврат первого элемента с его удалением
		CShape^ popFront() {
			CShape^ result = first->value;
			Node^ temp = first->next;

			delete first;
			first = temp;
			size--;
			return result;
		}


		// Возврат последнего элемента с его удалением
		CShape^ popBack() {
			CShape^ result = last->value;
			Node^ temp = first;
			while (temp->next != last) {
				temp = temp->next;
			}
			delete last;
			last = temp;
			size--;
			return result;
		}

		// Возврат n-ного элемента с его удалением
		CShape^ popNth(size_t position) {
			if (position == 0) {
				return popFront();
			}
			else if (position == size - 1) {
				return popBack();
			}
			else if (position > size - 1) {
				return nullptr;
			}

			Node^ temp = first, ^ prev = nullptr;
			for (int i = 0; i < position; i++) {
				prev = temp;
				temp = temp->next;
			}
			CShape^ result = temp->value;
			prev->next = temp->next;
			delete temp;

			size--;
			return result;
		}

		// Проверка на наличие
		bool isContain(const CShape^ value) {
			Node^ temp = first;
			while (temp != last) {
				if (temp->value == value)
					return true;
				temp = temp->next;
			}
			if (temp->value == value)
				return true;
			return false;
		}
	};
}